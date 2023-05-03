using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;
using Microsoft.EntityFrameworkCore;

namespace JamTeamFormingTool.Services
{
    public class RoleTemplateService
    {
        private MyDBContext _dbContext;

        public RoleTemplateService(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CanRegisterStagedOnTotalLimit()
        {
            return _dbContext.RoleTemplates
                .Count()
                < DBConstants.MaxRoleTemplates;
        }

        public bool CanRegisterStagedOnLimit()
        {
            return _dbContext.RoleTemplates
                .Where(t => t.IsStaged)
                .Count()
                < DBConstants.MaxStagingRoleTemplates;
        }

        public bool CanRegisterStagedOnName(string name)
        {
            return _dbContext.RoleTemplates
                .Where(t => t.Name == name)
                .Count() == 0;
        }

        public void RegisterStaged(RoleTemplate roleTemplate)
        {
            roleTemplate.IsStaged = true;
            roleTemplate.AuthorizePassCode = PassCode.Generate();
            //roleTemplate.AuthorizePassCode = "AAAA";
            _dbContext.RoleTemplates.Add(roleTemplate);
            _dbContext.SaveChanges();
        }

        public RoleTemplate? Retrieve(string name)
        {
            var maybe_template = _dbContext.RoleTemplates
                .Where(t => t.Name == name)
                .Include(t => t.Roles)
                .Include(t => t.CoverageSets)
                .ThenInclude(s => s.RoleCategories).ThenInclude(c => c.Roles)
                .AsNoTracking()
                .FirstOrDefault();
            if (maybe_template != null)
            {
                maybe_template.Email = null;
                maybe_template.AuthorizePassCode = null;
            }
            return maybe_template;
        }

        public bool PassCodeAttempt(RoleTemplate roleTemplate, string passCode)
        {
            RoleTemplate? maybe_template = _dbContext.RoleTemplates
                .Where(t => t.Name == roleTemplate.Name)
                .AsNoTracking()
                .FirstOrDefault();
            if (maybe_template == null)
            {
                return false;
            }
            else
            {
                return maybe_template.AuthorizePassCode == passCode;
            }
        }

        public void Submit(RoleTemplate roleTemplate)
        {
            var maybe_template = _dbContext.RoleTemplates
                .Where(t => t.Name == roleTemplate.Name)
                .FirstOrDefault();
            if (maybe_template != null)
            {
                maybe_template.IsStaged = false;
                _dbContext.SaveChanges();
            }
        }

        public bool CanDelete(RoleTemplate roleTemplate)
        {
            return _dbContext.JamTeamFormingSessions.Where(s => s.RoleTemplate == roleTemplate).Count() == 0;
        }

        public void Delete(RoleTemplate roleTemplate)
        {
            RoleTemplate? trackedRoleTemplate = _dbContext.RoleTemplates.Where(t => t.Name == roleTemplate.Name).FirstOrDefault();
            if (trackedRoleTemplate != null)
            {
                _dbContext.RoleTemplates.Remove(trackedRoleTemplate);
                _dbContext.SaveChanges();
            }
        }
    }
}
