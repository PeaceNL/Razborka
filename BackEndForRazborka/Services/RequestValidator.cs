using BackEndForRazborka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

//namespace BackEndForRazborka.Services
//{
//    public class RequestValidator
//    {
//        public static IActionResult ValidateModel(ModelStateDictionary modelState)
//        {
//            if (!modelState.IsValid)
//            {
//                return new BadRequestObjectResult(modelState);
//            }
//            return null;
//        }

//        // Метод для проверки наличия файлов
//        public static IActionResult ValidateFiles(List<IFormFile> files)
//        {
//            if (files == null || files.Count == 0)
//            {
//                return new BadRequestObjectResult("Необходимо загрузить хотя бы одно изображение.");
//            }
//            return null;
//        }

//        // Метод для проверки идентификатора пользователя в токене и авторизации
//        public static IActionResult ValidateUserClaims(ClaimsPrincipal user, int userId)
//        {
//            var currentUserIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (currentUserIdClaim == null)
//            {
//                return new UnauthorizedObjectResult("Авторизируйтесь!");
//            }

//            if (int.Parse(currentUserIdClaim) != userId)
//            {
//                return new ForbidResult("У вас нет на это доступа");
//            }
//            return null;
//        }

//        // Метод для проверки только авторизации
//        public static IActionResult CheckUserAuthentication(ClaimsPrincipal user)
//        {
//            var currentUserIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (currentUserIdClaim == null)
//            {
//                return new UnauthorizedObjectResult("Не удалось получить информацию о пользователе.");
//            }
//            return null;
//        }

//    }
//}
