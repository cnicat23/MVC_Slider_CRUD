using Microsoft.AspNetCore.Hosting;
using MVC_CRUD.Business.Exceptions;
using MVC_CRUD.Business.Extensions;
using MVC_CRUD.Business.Services.Abstract;
using MVC_CRUD.Core.Models;
using MVC_CRUD.Core.RepositoryAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_CRUD.Business.Services.Concretes
{
	public class SliderService : ISliderService
	{
		private readonly ISliderRepository _sliderRepository;
        private readonly IWebHostEnvironment _env;


        public SliderService(ISliderRepository sliderRepository, IWebHostEnvironment env)
        {
            _sliderRepository = sliderRepository;
			_env = env;
        }

		public async Task AddSliderAsync(Slider slider)
		{
			//if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg")
			//	throw new ImageContentTypeException("fayl formati duzgun deyil");

			//if (slider.ImageFile.Length > 2097152)
			//	throw new ImageSizeException("seklin olcusu maksimum 2mb ola biler");

			//string fileName = Guid.NewGuid().ToString() + Path.GetExtension(slider.ImageFile.FileName);
			//fileName = slider.ImageFile.FileName.Length > 100 ? slider.ImageFile.FileName.Substring(0, 55) : fileName + slider.ImageFile.FileName;

			//string path = _env.WebRootPath + "\\uploads\\sliders\\" + fileName;

			//using(FileStream fileStream = new FileStream(path, FileMode.Create))
			//{
			//	slider.ImageFile.CopyTo(fileStream);
			//}

			//slider.ImageUrl = fileName;


			slider.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads\sliders", slider.ImageFile);


			await _sliderRepository.AddAsync(slider);
			await _sliderRepository.CommitAsync();

		}

		public void DeleteSliderAsync(int id)
		{
			var existSlider = _sliderRepository.Get(x => x.Id == id);
			if (existSlider == null) throw new EntityNotFoundException("bele id movcud deyil");

			//string path = _env.WebRootPath + "\\uploads\\sliders\\" + existSlider.ImageUrl;

			//if (File.Exists(path)) throw new Exceptions.FileNotFoundException("fayl tapilmadi");

			//File.Delete(path);

			Helper.DeleteFile(_env.WebRootPath, @"uploads\sliders", existSlider.ImageUrl);


            _sliderRepository.Delete(existSlider);
			_sliderRepository.Commit();
		}

		public List<Slider> GetAllSliderAsync(Func<Slider, bool>? func = null)
		{
			return _sliderRepository.GetAll(func);
		}

		public Slider GetSliderAsync(Func<Slider, bool>? func = null)
		{
			return _sliderRepository.Get(func);
		}

		public void UpdateSliderAsync(int id, Slider newSlider)
		{
			var existSlider = _sliderRepository.Get(x => x.Id == id);
			if (existSlider == null) throw new NullReferenceException("duzgun id daxil et");

			if (newSlider.ImageFile != null)
			{
                //if (newSlider.ImageFile.ContentType != "image/png" && newSlider.ImageFile.ContentType != "image/jpeg")
                //    throw new ImageContentTypeException("fayl formati duzgun deyil");

                //if (newSlider.ImageFile.Length > 2097152) throw new ImageSizeException("Seklin olcusu 2mb ola biler");

                //string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newSlider.ImageFile.FileName);

                //string path = _env.WebRootPath + "\\uploads\\sliders\\" + fileName;

                //using (FileStream fileStream = new FileStream(path, FileMode.Create))
                //{
                //    newSlider.ImageFile.CopyTo(fileStream);
                //}

                //string oldPath = _env.WebRootPath + "\\uploads\\sliders\\" + existSlider.ImageUrl;

                //if (File.Exists(oldPath)) throw new Exceptions.FileNotFoundException("fayl tapilmadi");

                //File.Delete(oldPath);
                Helper.DeleteFile(_env.WebRootPath, @"uploads\sliders", existSlider.ImageUrl);

                existSlider.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads\sliders", newSlider.ImageFile);
            }

			existSlider.LifeCycle = newSlider.LifeCycle;
			existSlider.Title = newSlider.Title;
			existSlider.Description = newSlider.Description;
			existSlider.RedirectUrl = newSlider.RedirectUrl;
			_sliderRepository.Commit();
			


		}
	}
}
