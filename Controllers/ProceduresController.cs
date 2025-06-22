using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ApiRestNetNoxun.Data;
using ApiRestNetNoxun.Models;
using ApiRestNetNoxun.Dtos;

namespace ApiRestNetNoxun.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProceduresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProceduresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("field")]
        public async Task<IActionResult> CreateField([FromBody] FieldDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FieldName) || string.IsNullOrWhiteSpace(dto.DataType))
                return BadRequest("El nombre y tipo de dato del campo son obligatorios.");

            var field = new Field
            {
                FieldName = dto.FieldName,
                DataType = dto.DataType
            };

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Campo creado correctamente.", field });
        }

        [HttpPost("dataset")]
        public async Task<IActionResult> CreateDataset([FromBody] DataSetDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DataSetName))
                return BadRequest("El nombre del dataset es obligatorio.");

            var procedureExists = await _context.Procedures.AnyAsync(p => p.ProcedureID == dto.ProcedureID);
            var fieldExists = await _context.Fields.AnyAsync(f => f.FieldID == dto.FieldID);

            if (!procedureExists)
                return BadRequest($"No existe el procedimiento con ID {dto.ProcedureID}.");

            if (!fieldExists)
                return BadRequest($"No existe el campo con ID {dto.FieldID}.");

            var dataset = new DataSet
            {
                DataSetName = dto.DataSetName,
                Description = dto.Description,
                ProcedureID = dto.ProcedureID,
                FieldID = dto.FieldID,
                CreatedDate = DateTime.UtcNow
            };

            _context.DataSets.Add(dataset);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Dataset creado correctamente.", dataset });
        }

        [HttpPut("procedure/{id}")]
        public async Task<IActionResult> UpdateProcedure(int id, [FromBody] ProcedureDto dto)
        {
            if (id != dto.ProcedureID)
                return BadRequest("El ID en la URL no coincide con el ID en el cuerpo.");

            var proc = await _context.Procedures.FindAsync(id);
            if (proc == null)
                return NotFound($"No se encontró el procedimiento con ID {id}.");

            proc.ProcedureName = dto.ProcedureName;
            proc.Description = dto.Description;
            proc.LastModifiedUserID = dto.LastModifiedUserID;
            proc.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Procedimiento actualizado correctamente.", procedure = proc });
        }

        [HttpDelete("dataset/{id}")]
        public async Task<IActionResult> DeleteDataset(int id)
        {
            var ds = await _context.DataSets.FindAsync(id);
            if (ds == null)
                return NotFound($"No se encontró el dataset con ID {id}.");

            _context.DataSets.Remove(ds);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Dataset eliminado correctamente." });
        }
    }
}

