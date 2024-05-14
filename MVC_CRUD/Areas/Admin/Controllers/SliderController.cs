using Microsoft.AspNetCore.Mvc;
using MVC_CRUD.Business.Exceptions;
using MVC_CRUD.Business.Services.Abstract;
using MVC_CRUD.Business.Services.Concretes;
using MVC_CRUD.Core.Models;
using FileNotFoundException = MVC_CRUD.Business.Exceptions.FileNotFoundException;

namespace MVC_CRUD.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SliderController : Controller
	{
		private readonly ISliderService _sliderService;

		public SliderController(ISliderService sliderService)
		{
			_sliderService = sliderService;
		}

		public IActionResult Index()
		{
			var sliders = _sliderService.GetAllSliderAsync();
            return View(sliders);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Slider slider)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			try
			{
				await _sliderService.AddSliderAsync(slider);
			}
			catch (ImageContentTypeException ex)
			{
				ModelState.AddModelError("ImageFile", ex.Message);
				return View();
			}
			catch (FileNotFoundException ex)
			{
				ModelState.AddModelError("ImageFile", ex.Message);
				return View();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			return RedirectToAction("Index");
		}

		public IActionResult Update(int id)
		{
			var existSlider = _sliderService.GetSliderAsync(x => x.Id == id);
            if (existSlider == null) return NotFound();
            
            return View(existSlider);
		}

		[HttpPost]
		public IActionResult Update(Slider slider) 
		{
			if (slider == null)
			{
				return NotFound();
			}
			else if (!ModelState.IsValid)
			{
				return View();
			}
			try
			{
				_sliderService.UpdateSliderAsync(slider.Id, slider);
			}
			catch (ImageContentTypeException ex)
			{
				ModelState.AddModelError("ImageFile", ex.Message);
				return View();
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound();
			}
			catch (Business.Exceptions.FileNotFoundException ex)
			{
				ModelState.AddModelError("ImageFile", ex.Message);
				return View();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}


			return RedirectToAction("Index");
		}

		public IActionResult Delete(int id)
        {
            var existSlider = _sliderService.GetSliderAsync(x => x.Id == id);
            if (existSlider == null) return NotFound();
            return View(existSlider);
        }

		[HttpPost]
		public IActionResult DeletePost(int id)
		{
            try
            {
                _sliderService.DeleteSliderAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }
	}
}
