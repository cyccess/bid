using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using OAuth.Core.Interfaces;
using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using OAuth.Service.ModelDto;
using Webdiyer.WebControls.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using OAuth.Service.Common;
using System.Web;

namespace OAuth.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public Supplier Get(int id)
        {
            return _repo.GetById<Supplier>(id);
        }

        public void Add(SupplierDto model)
        {
            var entity = Mapper.Map<SupplierDto, Supplier>(model);
            string pwd = Guid.NewGuid().ToString("N");
            entity.Password = EncryptHelper.Encrypt(pwd.Substring(0, 8));
            entity.IsEnabled = true;
            _unitOfWork.RegisterNew(entity);
            _unitOfWork.Commit();
            //供应商类型关系
            ModesRelation(entity.Id, model.SupplierModeIds);

            EmailNotice(entity.Id);
        }

        public bool CheckEmail(string email)
        {
            return _repo.GetAll<Supplier>().Any(s => s.Email == email.Trim());
        }

        /// <summary>
        /// 供应商类型关系
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="supplierModes"></param>
        private void ModesRelation(int sid, int[] supplierModes)
        {
            foreach (var mid in supplierModes)
            {
                var entity = new SupplierMode
                {
                    SupplierId = sid,
                    ModeId = mid
                };
                _unitOfWork.RegisterNew(entity);
            }
            _unitOfWork.Commit();
        }

        private List<SupplierMode> GetModesRelation(int sid)
        {
            var modes = _repo.GetAll<SupplierMode>()
                .Include(s => s.Mode)
                .Where(sm => sm.SupplierId == sid)
                .ToList();
            return modes;
        }

        public SupplierDto GetSupplierById(int id)
        {
            var entity = _repo.GetById<Supplier>(id);
            var model = Mapper.Map<Supplier, SupplierDto>(entity);
            var supplierModes = GetModesRelation(id);

            model.Password = EncryptHelper.Decrypt(model.Password);
            model.SupplierModeIds = supplierModes.Select(s => s.ModeId).ToArray();
            model.ModesName = string.Join(",", supplierModes.Select(s => s.Mode.ModeName));

            return model;
        }

        public IPagedList<Supplier> GetSuppliers(int pageIndex, int ModeId, string SupplierName)
        {
            var entities = _repo.GetAll<Supplier>().Include(s => s.SupplierModes);

            if (!string.IsNullOrEmpty(SupplierName))
            {
                entities = entities.Where(s => s.SupplierName.Contains(SupplierName));
            }

            if (ModeId > 0)
            {
                entities = entities.Where(s => s.SupplierModes.Any(sm => sm.ModeId == ModeId));
            }

            return entities.OrderBy(u => u.Id).ToPagedList(pageIndex, 10);
        }

        public void Update(SupplierDto model)
        {
            var entity = Get(model.Id);
            string pwd = entity.Password;

            entity = Mapper.Map(model, entity);
            entity.Password = pwd;
            _unitOfWork.RegisterDirty(entity);

            var modes = GetModesRelation(model.Id);

            var lt = (from p in modes join l in model.SupplierModeIds on p.ModeId equals l into dt from it in dt.DefaultIfEmpty() select p).ToList();
            lt.ForEach(p => _unitOfWork.RegisterDeleted(p));

            var li = (from p in model.SupplierModeIds join l in modes on p equals l.ModeId into dt from it in dt.DefaultIfEmpty() select p).ToList();
            li.ForEach(p => { _unitOfWork.RegisterNew(new SupplierMode { SupplierId = model.Id, ModeId = p }); });

            _unitOfWork.Commit();
        }

        public void Delate(int id)
        {
            //DeleteMode(id);
            var entity = Get(id);
            entity.IsEnabled = !entity.IsEnabled;
            _unitOfWork.Commit();
        }

        private void DeleteMode(int sid)
        {
            var modes = _repo.GetAll<SupplierMode>()
               .Where(sm => sm.SupplierId == sid)
               .ToList();
            foreach (var mode in modes)
            {
                _unitOfWork.RegisterDeleted(mode);
            }
        }

        public SupplierDto Login(string username, string password)
        {
            password = EncryptHelper.Encrypt(password);
            var entity = _repo.GetAll<Supplier>().SingleOrDefault(u => u.Email == username && u.Password == password);

            if (entity != null)
            {
                var userDto = Mapper.Map<Supplier, SupplierDto>(entity);
                userDto.UserName = userDto.SupplierName;
                return userDto;
            }

            return new SupplierDto();
        }

        //物料竞价
        public void MaterialQuote(ItemQuoteDto model)
        {
            var entity = Mapper.Map<ItemQuoteDto, ItemQuote>(model);
            entity.InputTime = DateTime.Now;

            if (entity.Id != 0)
            {
                _unitOfWork.RegisterDirty(entity);
            }
            else
            {
                _unitOfWork.RegisterNew(entity);
            }

            _unitOfWork.Commit();
        }

        public ItemQuote GetItemQuote(int id)
        {
            return _repo.GetAll<ItemQuote>().SingleOrDefault(p => p.Id == id);
        }

        public ResultModel UpdatePassword(int supplierId, string oldPwd, string newPwd)
        {
            ResultModel resultModel = new ResultModel(-100, "旧密码验证错误");
            oldPwd = EncryptHelper.Encrypt(oldPwd);

            var entity = _repo.GetAll<Supplier>().SingleOrDefault(s => s.Id == supplierId && s.Password == oldPwd);

            if (entity != null)
            {
                entity.Password = EncryptHelper.Encrypt(newPwd);
                _unitOfWork.Commit();
                resultModel = new ResultModel(200, "密码修改成功");
            }

            return resultModel;
        }

        public void EmailNotice(int supplierId)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["appurl"];
            EmailService service = new EmailService();
            var entity = Get(supplierId);
            entity.NoticeTime = DateTime.Now;
            _unitOfWork.Commit();

            string url = $"{path}/Home/SupplierActive?supplierId=" + HttpUtility.UrlEncode(EncryptHelper.Encrypt(supplierId.ToString()));
            StringBuilder content = new StringBuilder();
            content.AppendFormat("<p>尊敬的供应商您好：<p>");
            content.Append("  感谢您使用我公司的网上电子投标竞价系统，您的登录账号和密码如下所示，请点击用户激活链接，如您激活，代表您已认可您的网上电子报价真实有效，并保证不进行恶意投标。");
            content.Append("如您恶意投标，又以种种理由拒绝履约签订合同，将会被剔除合格供应商，不能再参与报价，谢谢合作！");
            content.AppendFormat("<p>登录帐号：{0}<p>", entity.Email);
            content.AppendFormat("<p>登录密码：{0}<p>", EncryptHelper.Decrypt(entity.Password));
            content.AppendFormat("<p style='width:100%;'>用户激活链接：{0}</p>", url);
            content.Append("<div style='width:100%;text-align=center;margin-top:30px;'>青岛武晓集团股份有限公司</div>");

            service.SendEmail(entity.Email, "供应商账户激活", content.ToString());
        }

        public void SupplierActive(Supplier entity)
        {
            entity.IsActivated = true;
            _unitOfWork.RegisterDirty(entity);
            _unitOfWork.Commit();
        }

        public void ForgetPassword(int supplierId)
        {
            var entity = Get(supplierId);
            if (entity != null)
            {
                EmailService service = new EmailService();
                StringBuilder content = new StringBuilder();
                content.AppendFormat("<p><strong>{0}：</strong><p>", entity.SupplierName);
                content.Append("  您的竞价账号密码为：").Append(EncryptHelper.Decrypt(entity.Password)).Append("，请妥善保管！<br /><div style='width:100%;float:right;'>青岛武晓集团");
                service.SendEmail(entity.Email, "供应商账户密码", content.ToString());
            }
        }
    }
}
