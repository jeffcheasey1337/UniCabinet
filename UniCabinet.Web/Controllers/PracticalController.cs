﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniCabinet.Application.UseCases.PracticalUseCase;
using UniCabinet.Core.DTOs.PracticalManagement;
using UniCabinet.Core.Models.ViewModel.Practical;

public class PracticalController : Controller
{
    private readonly IMapper _mapper;

    public PracticalController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> PracticalsListAsync(int detailId, [FromServices] GetPracticalsListDataUseCase practicalsListUseCase)
    {
        var result = await practicalsListUseCase.ExecuteAsync(detailId);

        ViewBag.Discipline = result.DisciplineName;
        ViewBag.DisciplineDetailId = detailId;
        ViewBag.MaxPracticals = result.MaxPracticals;

        var practicalListVM = _mapper.Map<List<PracticalListVM>>(result.PracticalDTO);

        return View(practicalListVM);
    }

    [HttpGet]
    public IActionResult PracticalAddModal(int id)
    {
        var viewModel = new PracticalAddVM
        {
            DisciplineDetailId = id
        };
        return PartialView("_PracticalAddModal", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddPracticalAsync(
        PracticalAddVM viewModel,
        [FromServices] AddPracticalUseCase addPracticalUseCase)
    {
        if (ModelState.IsValid)
        {
            var practicalDTO = _mapper.Map<PracticalDTO>(viewModel);

            var success = await addPracticalUseCase.ExecuteAsync(practicalDTO, ModelState);

            if (success)
            {
                return RedirectToAction("PracticalsList", new { detailId = viewModel.DisciplineDetailId });
            }
        }

        return PartialView("_PracticalAddModal", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> PracticalEditModal(int id, [FromServices] GetPracticalForEditUseCase getPracticalForEditUseCase)
    {
        var practicalDTO = await getPracticalForEditUseCase.ExecuteAsync(id);
        var practicalVM = _mapper.Map<PracticalEditVM>(practicalDTO);

        return PartialView("_PracticalEditModal", practicalVM);
    }

    [HttpPost]
    public async Task<IActionResult> EditPracticalAsync(
        PracticalEditVM viewModel,
        [FromServices] UpdatePracticalUseCase updatePracticalUseCase)
    {
        if (ModelState.IsValid)
        {
            var practicalDTO = _mapper.Map<PracticalDTO>(viewModel);

            await updatePracticalUseCase.ExecuteAsync(practicalDTO);
            var disciplineDetailId = viewModel.DisciplineDetailId;

            return RedirectToAction("PracticalsList", new { detailId = viewModel.DisciplineDetailId });
        }

        return PartialView("_PracticalEditModal", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> PracticalAttendance(
        int practicalId,
        [FromServices] GetPracticalAttendanceUseCase getPracticalAttendanceUseCase)
    {
        var result = await getPracticalAttendanceUseCase.ExecuteAsync(practicalId);
        var attendanceVM = _mapper.Map<PracticalAttendanceVM>(result);
        if (attendanceVM == null)
        {
            return NotFound();
        }
        return View("PracticalAttendance", attendanceVM);
    }

    [HttpPost]
    public async Task<IActionResult> SaveAttendance(
        PracticalAttendanceVM viewModel,
        [FromServices] SavePracticalAttendanceUseCase savePracticalAttendanceUseCase)
    {
        var practicalAttendanceDTO = _mapper.Map<PracticalAttendanceDTO>(viewModel);
        await savePracticalAttendanceUseCase.ExecuteAsync(practicalAttendanceDTO);
        return RedirectToAction("PracticalsList", new { detailId = viewModel.DisciplineDetailId });
    }
}
