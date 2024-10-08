using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using AirCinelMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        var users = _userHelper.GetAllUsersWithCity();
        var loggedUser = _userHelper.GetUserByEmailAsync(User.Identity.Name).Result;
        //users = users.Where(u => u.Id != loggedUser.Id);

        var model = new List<EditUserRolesViewModel>();
        foreach (var user in users)
        {
            var roles = _userHelper.GetRolesAsync(user).Result;

            model.Add(new EditUserRolesViewModel
            {
                UserId = user.Id,
                Username = user.FullName,
                Email = user.Email,
                Roles = roles.Select(r => new UserRoleViewModel { RoleName = r }).ToList(),
                SelectedRole = roles.FirstOrDefault(),
                Address = user.Address,
                CityName = user.City?.Name,
                ImageFullPath = user.ImageFullPath
            });
        }
        return View(model);
    }


    public async Task<IActionResult> EditUserRoles(string id)
    {
        var user = await _userHelper.GetUserByIdAsync(id);
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

        return RedirectToAction("Index");
    }



    public IActionResult RegisterEmployee()
    {
        var model = new RegisterNewUserViewModel
        {
            Countries = _countryRepository.GetComboCountries(),
            Cities = _countryRepository.GetComboCities(1)
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterEmployee(RegisterNewUserViewModel model)
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

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

                if (result != IdentityResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "The employee couldn't be created.");
                    return View(model);
                }

                return RedirectToAction("Index", "Admin");
            }
        }

        return View(model);
    }

    public IActionResult RegisterCustomer()
    {

       var model = new RegisterNewUserViewModel
        {
            Countries = _countryRepository.GetComboCountries(),
            Cities = _countryRepository.GetComboCities(1)
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(RegisterNewUserViewModel model)
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
                await _userHelper.AddUserToRoleAsync(user, "Customer");

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);


                if (result != IdentityResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "The customer couldn't be created.");
                    return View(model);
                }

                return RedirectToAction("Index", "Admin");
            }
        }

        return View(model);
    }


    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userHelper.GetUserByIdAsync(id);
        var model = new ChangeUserViewModel();
        if (user != null)
        {
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Address = user.Address;
            model.PhoneNumber = user.PhoneNumber;

            var city = await _countryRepository.GetCityAsync(user.CityId);
            if (city != null)
            {
                var country = await _countryRepository.GetCountryAsync(city);
                if (country != null)
                {
                    model.CountryId = country.Id;
                    model.CityId = city.Id;

                    // Populando as listas de países e cidades
                    model.Countries = _countryRepository.GetComboCountries();
                    model.Cities = _countryRepository.GetComboCities(country.Id);
                }
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(ChangeUserViewModel model, string id)
    {
        if (ModelState.IsValid)
        {
            var user = await _userHelper.GetUserByIdAsync(id);
            if (user != null)
            {
                var city = await _countryRepository.GetCityAsync(model.CityId);

                // Atualiza os dados do usuário
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.CityId = model.CityId;
                user.City = city;

                // Verifica se há uma nova imagem para upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    user.ImageId = imageId;
                }

                var response = await _userHelper.UpdateUserAsync(user);
                if (response.Succeeded)
                {
                    ViewBag.UserMessage = "User updated!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault()?.Description);
                }
            }
        }

        // Recarrega as listas de países e cidades
        model.Countries = _countryRepository.GetComboCountries();
        if (model.CountryId != 0)
        {
            model.Cities = _countryRepository.GetComboCities(model.CountryId);
        }

        return View(model);
    }


    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return new NotFoundViewResult("UserNotFound");
        }

        var user = await _userHelper.GetUserByIdAsync(id);
        if (user == null)
        {
            return new NotFoundViewResult("UserNotFound");
        }

        var city = await _countryRepository.GetCityAsync(user.CityId);
        if (city == null)
        {
            return new NotFoundViewResult("UserNotFound");
        }
        user.City = city;

        return View(user);
    }


    public async Task<IActionResult> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new NotFoundViewResult("UserNotFound");
        }

        var user = await _userHelper.GetUserByIdAsync(id);
        if (user == null)
        {
            return new NotFoundViewResult("UserNotFound");
        }

        try
        {
            await _userHelper.RemoveRolesAsync(user, await _userHelper.GetRolesAsync(user));
            await _userHelper.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
            {
                ViewBag.ErrorTitle = $"This User is probably being used!";
                ViewBag.ErrorMessage = $"This User can't be deleted because he has tickets bought.</br></br>";
            }

            return View("Error");
        }
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
