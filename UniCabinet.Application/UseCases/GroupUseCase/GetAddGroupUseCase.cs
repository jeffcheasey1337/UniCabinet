﻿using UniCabinet.Application.Interfaces.Repository;
using System;
using UniCabinet.Core.DTOs.CourseManagement;

namespace UniCabinet.Application.UseCases.GroupUseCase
{
    public class GetGroupAddModalUseCase
    {
        private readonly ISemesterRepository _semesterRepository;

        public GetGroupAddModalUseCase(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }

        public async Task<GroupDTO> Execute()
        {
            var currentSemester = await _semesterRepository.GetCurrentSemesterAsync(DateTime.Now);
            return new GroupDTO
            {
                SemesterId = currentSemester?.Number ?? 0
            };
        }
    }
}
