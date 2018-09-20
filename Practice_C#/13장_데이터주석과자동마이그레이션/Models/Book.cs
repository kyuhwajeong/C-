using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;  // 참조 라이브러리 추가

namespace SampleEntityFramework.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [MaxLength(16)]
        public string Publisher { get; set; }
        public int PublishedYear { get; set; }
        public virtual Author Author { get; set; }
    }
}
