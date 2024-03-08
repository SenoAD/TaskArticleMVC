using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;

namespace SampleMVC.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ICategoryBLL _categoryBLL;
        private readonly IArticleBLL _articleBLL;
        public ArticlesController(ICategoryBLL categoryBLL, IArticleBLL articleBLL)
        {
            _categoryBLL = categoryBLL;
            _articleBLL = articleBLL;
            //_articleBLL = articleBLL;
        }
        //private ArticlesController() { }

        public class MyCombinedViewModel
        {
            public object DataObject1 { get; set; }
            public object DataObject2 { get; set; }
        }

        public IActionResult Index()
        {
            var data1 = _categoryBLL.GetAll();
    
            //var data2 = _articleBLL.GetArticleByCategory(1);

            var viewModel = new MyCombinedViewModel
            {
                DataObject1 = data1,
                //DataObject2 = data2
            };

            return View(viewModel);
        }

        
        public IActionResult Paging(string selectedOption, int pageNumber = 1, int pageSize = 5, string act = "")
        {
            ViewData["ID"] = selectedOption;
            var data2 = _articleBLL.GetWithPaging(pageNumber, pageSize, int.Parse(selectedOption));
            var maxsize = _articleBLL.GetCountCategories(int.Parse(selectedOption));
            var data1 = _categoryBLL.GetAll();


            var viewModel = new MyCombinedViewModel
            {
                DataObject1 = data1,
                DataObject2 = data2
            };

            if (act == "next")
            {
                if (pageNumber * pageSize < maxsize)
                {
                    pageNumber += 1;
                }
                ViewData["pageNumber"] = pageNumber;
            }
            else if (act == "prev")
            {
                if (pageNumber > 1)
                {
                    pageNumber -= 1;
                }
                ViewData["pageNumber"] = pageNumber;
            }
            else
            {
                ViewData["pageNumber"] = 2;
            }

            ViewData["pageSize"] = pageSize;

            return View(viewModel);
        }

        public IActionResult Create(int id) 
        { 
            return View(id);
        }

        [HttpPost]
        public IActionResult Create(ArticleCreateDTO articleCreate, IFormFile imageArticle)
        {
            if (imageArticle != null && imageArticle.Length > 0)
            {

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageArticle.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "picture", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageArticle.CopyTo(stream);
                }

                articleCreate.Pic = fileName;
            }

            _articleBLL.Insert(articleCreate);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var model = _articleBLL.GetArticleById(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(ArticleUpdateDTO articleUpdate)
        {
            _articleBLL.Update(articleUpdate);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            _articleBLL.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
