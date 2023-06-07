using Microsoft.AspNetCore.Identity;

namespace TryToAddMultipleDBContext.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string UserType { get; set; }
    public string RoleName { get; set; }
    public int EntityId { get; set; }
    public int BranchId { get; set; }
    public int CompanyId { get; set; }
    public bool IsActive { get; set; } = true;
    public string AvartarUrl { get; set; }
    public string TenantId { get; set; }
}
