using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AddressBook.Data;
using AddressBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AddressBook.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ABUser> _userManager;
        public ContactsController(ApplicationDbContext context, UserManager<ABUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {



            return View(await _context.Contact.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,ImageData,Address1,Address2,City,State,ZipCode,PhoneNumber")] Contact contact, IFormFile imageData)
        {
            if (ModelState.IsValid)
            {
                contact.OwnerId = _userManager.GetUserId(User);
                contact.ContactUpdated = DateTime.Now;
                contact.PhoneNumber = Convert.ToInt64(contact.PhoneNumber).ToString("###-###-####");
                if (imageData != null)
                {
                    var ms = new MemoryStream();
                    imageData.CopyTo(ms);
                    byte[] bytes = ms.ToArray();

                    ms.Close();
                    ms.Dispose();

                    var binary = Convert.ToBase64String(bytes);
                    var ext = Path.GetExtension(imageData.FileName);

                    contact.ImagePath = $"data:image/{ext};base64,{binary}";
                    contact.ImageData = bytes;
                }




                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,FirstName,LastName,Email,ImagePath,ImageData,Address1,Address2,City,State,ZipCode,PhoneNumber,ContactUpdated")] Contact contact, IFormFile imageData)
        {

            contact.ContactUpdated = DateTime.Now;
            if (id != contact.Id)
            {
                return NotFound();
            }


                var ms = new MemoryStream();
                imageData.CopyTo(ms);
                byte[] bytes = ms.ToArray();

                ms.Close();
                ms.Dispose();

                var binary = Convert.ToBase64String(bytes);
                var ext = Path.GetExtension(imageData.FileName);

                contact.ImagePath = $"data:image/{ext};base64,{binary}  ";
                contact.ImageData = bytes;

                _context.Update(contact);
                await _context.SaveChangesAsync();


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.Id == id);
        }
    }
}
