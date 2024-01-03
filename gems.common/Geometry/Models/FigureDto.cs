using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gems.common.Geometry.Figures;

namespace gems.common.Geometry.Models
{
    public class FigureDto
	{
		[Key]
		[DatabaseGenerated( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }

		[Column( "Geometry", TypeName= "TEXT" )]
		public IFigure Geometry { get; set; }
	}
}
