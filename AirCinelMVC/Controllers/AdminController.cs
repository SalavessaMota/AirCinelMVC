using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using AirCinelMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserHelper _userHelper;
    private readonly IBlobHelper _blobHelper;
    private readonly ICountryRepository _countryRepository;

    public AdminController(
        IUserHelper userHelper,
        IBlobHelper blobHelper,
        ICountryRepository countryRepository)
    {
        _userHelper = userHelper;
        _blobHelper = blobHelper;
        _countryRepository = countryRepository;
    }


    public IActionResult Index()
    {
        return View();
    }


    public IActionResult ManageUsers()
    {
        var users = _userHelper.GetAllUsers();
        return View(users);
    }


    public async Task<IActionResult> EditUserRoles(string userEmail)
    {
        var user = await _userHelper.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return new NotFoundViewResult("UserNotFound");
        }

        var userRoles = await _userHelper.GetRolesAsync(user);
        var userRole = userRoles.FirstOrDefault();

        var model = new EditUserRolesViewModel
        {
            UserId = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Roles = _userHelper.GetAllRoles().Select(u => new UserRoleViewModel
            {
                RoleName = u.Name
            }).ToList(),
            SelectedRole = userRole
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> EditUserRoles(EditUserRolesViewModel model)
    {
        var user = await _userHelper.GetUserByEmailAsync(model.Email);
        if (user == null)
        {
            return new NotFoundViewResult("UserNotFound");
        }

        if (string.IsNullOrEmpty(model.SelectedRole))
        {
            ModelState.AddModelError("", "Please select a role.");
            model.Roles = _userHelper.GetAllRoles().Select(u => new UserRoleViewModel
            {
                RoleName = u.Name
            }).ToList();
            return View(model);
        }

        var roles = await _userHelper.GetRolesAsync(user);
        var result = await _userHelper.RemoveRolesAsync(user, roles);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Failed to remove existing roles.");
            model.Roles = _userHelper.GetAllRoles().Select(u => new UserRoleViewModel
            {
                RoleName = u.Name
            }).ToList();
            return View(model);
        }

        var addResult = await _userHelper.AddUserToRoleAsync(user, model.SelectedRole);
        if (!addResult.Succeeded)
        {
            ModelState.AddModelError("", "Failed to add the selected role.");
            model.Roles = _userHelper.GetAllRoles().Select(u => new UserRoleViewModel
            {
                RoleName = u.Name
            }).ToList();
            return View(model);
        }

        return RedirectToAction("ManageUsers");
    }



    public IActionResult RegisterEmployee()
    {
        var model = new RegisterEmployeeViewModel
        {
            Countries = _countryRepository.GetComboCountries(),
            Cities = _countryRepository.GetComboCities(1)
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterEmployee(RegisterEmployeeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user == null)
            {
                var city = await _countryRepository.GetCityAsync(model.CityId);

                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Username,
                    UserName = model.Username,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    CityId = model.CityId
                };

                
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    user.ImageId = imageId;
                }

                var result = await _userHelper.AddUserAsync(user, model.Password);
                await _userHelper.AddUserToRoleAsync(user, "Employee");
                if (result != IdentityResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "The employee couldn't be created.");
                    return View(model);
                }

                return RedirectToAction("ManageUsers", "Admin");
            }
        }

        return View(model);
    }

    public IActionResult UserNotFound()
    {
        return View();
    }


    [HttpPost]
    [Route("Admin/GetCitiesAsync")]
    public async Task<IActionResult> GetCitiesAsync(int countryId)
    {
        var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);
        return Json(country.Cities.OrderBy(c => c.Name).Select(c => new
        {
            Id = c.Id,
            Name = c.Name
        }));
    }
}
