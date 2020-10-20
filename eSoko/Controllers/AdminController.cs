using eSoko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace eSoko.Controllers
{
    public class AdminController : Controller
    {
        MarketingEntities1 db = new MarketingEntities1();
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(dboadmin avm)
        {
            dboadmin ad = db.dboadmins.Where(x => x.adminusername == avm.adminusername && x.adminpassword == avm.adminpassword).SingleOrDefault();
            if (ad != null)
            {
                Session["adminId"] = ad.adminId.ToString();
                return RedirectToAction("CreateCategory");
            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
        }
        public ActionResult CreateCategory()
        {
            if (Session["adminId"]==null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(Category cvm,HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                Category cat = new Category();
                cat.catname = cvm.catname;
                cat.catimage = path;
                cat.catstatus = 1;
                cat.catfkid = Convert.ToInt32(Session["adminId"].ToString());
                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("ViewCategory");
            }
            return View();

        }
        public ActionResult ViewCategory(int?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Categories.Where(x => x.catstatus == 1).OrderByDescending(x => x.catid).ToList();
            IPagedList<Category> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }
        
        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = System.IO.Path.Combine(Server.MapPath("~/Content/upload"), random + System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + System.IO.Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
            }
        }
}