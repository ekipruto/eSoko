using eSoko.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eSoko.Controllers
{
    public class UserController : Controller
    {
        MarketingEntities1 db = new MarketingEntities1();
        // GET: User
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User uvm, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                User u = new User();
                u.username = uvm.username;
                u.userimage = path;
                u.userpassword = uvm.userpassword;
                u.useremail = uvm.useremail;
                u.usercontact = uvm.usercontact;
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();

        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User avm)
        {
            User u = db.Users.Where(x => x.username == avm.username && x.userpassword == avm.userpassword).SingleOrDefault();
            if (u != null)
            {
                Session["userid"] = u.userid.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
        }
        public ActionResult Index(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Categories.Where(x => x.catstatus == 1).OrderByDescending(x => x.catid).ToList();
            IPagedList<Category> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }

        public ActionResult ViewCategory(int? page)
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
        public ActionResult AddProduct()
        {
            List<Category> li = db.Categories.ToList();
            ViewBag.categorylist = new SelectList(li, "catid", "catname");
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product pvm, HttpPostedFileBase imgfile)
        {
            List<Category> li = db.Categories.ToList();
            ViewBag.categorylist = new SelectList(li, "catid", "catname");

            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                Product p = new Product();
                p.proname = pvm.proname;
                p.proimage = path;
                p.prodescr = pvm.prodescr;
                p.procontact = pvm.procontact;
                p.proprice = pvm.proprice;
                p.profkcatid = pvm.profkcatid;
                p.profkuserid = Convert.ToInt32(Session["userid"].ToString());
                db.Products.Add(p);
                db.SaveChanges();
                Response.Redirect("Index");
            }
            return View();

        }
        public ActionResult ProductDetails(int? id, int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.Where(x => x.profkcatid == id).OrderByDescending(x => x.proid).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }
        public ActionResult ProductSearch(int? id, int? page, string search)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.Where(x => x.proname.Contains(search)).OrderByDescending(x => x.proid).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }

        public ActionResult ViewItem(int? id)
        {
            ViewItemModel VIM = new ViewItemModel();
            Product p = db.Products.Where(x => x.proid == id).SingleOrDefault();
            VIM.proid = p.proid;
            VIM.proname = p.proname;
            VIM.proimage = p.proimage;
            VIM.proprice = p.proprice;
            Category cat = db.Categories.Where(x => x.catid == p.profkcatid).SingleOrDefault();
            VIM.catname = cat.catname;
            User u = db.Users.Where(x => x.userid == p.profkuserid).SingleOrDefault();
            VIM.username = u.username;
            VIM.userimage = u.userimage;
            VIM.usercontact = u.usercontact;
            VIM.profkuserid=u.userid;


            return View(VIM);
        }
        public ActionResult Signout()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("index");
        }
        public ActionResult DeleteItem(int?id)
        {
            Product p = db.Products.Where(x => x.proid == id).SingleOrDefault();
            db.Products.Remove(p);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}