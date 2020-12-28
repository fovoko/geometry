using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using gems.common.Geometry;

namespace Gems.SqliteDb.Models
{
	public class FigureDto
	{
		[Key]
		[DatabaseGenerated( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }

		[Column( "Geometry", TypeName= "TEXT" )]
		public Figure Geometry { get; set; }
	}
}
