﻿using AutoMapper;
using FullstackProjectWeek2.Data.DAL.IRepository;
using FullstackProjectWeek2.Domain;
using FullstackProjectWeek2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FullstackProjectWeek2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudent _student;
        private readonly IMapper _mapper;

        public StudentsController(IStudent student, IMapper mapper)
        {
            _student = student;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentReadDTO>> GetAll()
        {
            var results = await _student.GetAll();
            var studentReadDTOs = _mapper.Map<IEnumerable<StudentReadDTO>>(results);
            return studentReadDTOs;
        }

        [HttpGet("{id}")]
        public async Task<StudentReadDTO> Get(int id)
        {
            var result = await _student.GetById(id);
            if (result == null) throw new Exception($"Student ID: {id} tidak ditemukan");
            var studentDTO = _mapper.Map<StudentReadDTO>(result);

            return studentDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Insert(StudentCreateDTO studentCreateDTO)
        {
            try
            {
                var newCourse = _mapper.Map<Student>(studentCreateDTO);
                var result = await _student.Insert(newCourse);
                var studentReadDTO = _mapper.Map<StudentReadDTO>(result);

                return CreatedAtAction("Get", new { id = result.ID }, studentReadDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}