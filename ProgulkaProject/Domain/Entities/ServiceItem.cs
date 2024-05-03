using System.ComponentModel.DataAnnotations;

namespace ProgulkaProject.Domain.Entities
{
    public class ServiceItem : EntityBase
    {
        [Required (ErrorMessage = "Заполните название услуги")]

        [Display(Name = "Название услуги (заголовок)")]
        public override string Title { get; set; } = "Информационная страница";


        [Display(Name = "Кратое описание услуги")]
        public override string Subtitle { get; set; }

        [Display(Name = "Полное описание услуги")]
        public override string Text { get; set; }


    }
}
