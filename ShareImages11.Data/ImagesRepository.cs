using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareImages11.Data
{
    public class ImagesRepository
    {
        private string _connString;

        public ImagesRepository(string connString)
        {
            _connString = connString;
        }

        public void AddImage(Image image)
        {
            using var context = new ImagesDataContext(_connString);
            context.Images.Add(image);
            context.SaveChanges();
        }

        public List<Image> GetAllImages()
        {
            using var context = new ImagesDataContext(_connString);
            return context.Images.OrderByDescending(i => i.DateTimeUploaded).ToList();
        }

        public Image GetImageById(int id)
        {
            using var context = new ImagesDataContext(_connString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }

        public void LikeImage(int id)
        {
            using var context = new ImagesDataContext(_connString);
            var image = context.Images.FirstOrDefault(i => i.Id == id);
            if (image != null)
            {
                image.Likes++;
            }
            context.SaveChanges();
        }

        public int GetLikes(int id)
        {
            using var context = new ImagesDataContext(_connString);
            var image = context.Images.FirstOrDefault(i => i.Id == id);
            if (image == null)
            {
                return 0;
            }
            return image.Likes;
        }
    }
}
