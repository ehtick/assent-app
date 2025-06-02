﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.CFS.Approvals.SupportService.API.Controllers.api.v1;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CFS.Approvals.SupportServices.Helper.Interface;
using Microsoft.CFS.Approvals.SupportServices.Helper.ModelBinder;
using Newtonsoft.Json.Linq;

/// <summary>
/// The Tenant OnBoarding Controller
/// </summary>
[Route("api/v1/TenantOnBoarding")]
[ApiController]
public class TenantOnBoardingController : ControllerBase
{
    /// <summary>
    /// The tenant on-boarding helper
    /// </summary>
    private readonly ITenantOnBoardingHelper _tenantOnBoardingHelper;

    /// <summary>
    /// Constructor of TenantOnBoardingController
    /// </summary>
    /// <param name="tenantOnBoardingHelper"></param>
    public TenantOnBoardingController(ITenantOnBoardingHelper tenantOnBoardingHelper)
    {
        _tenantOnBoardingHelper = tenantOnBoardingHelper;
    }

    /// <summary>
    /// Submit tenant on-boarding files
    /// </summary>
    /// <param name="TenantDetails"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("{env}")]
    public async Task<IActionResult> Post([ModelBinder(BinderType = typeof(JsonModelBinder))] JObject TenantDetails)
    {
        try
        {
            await _tenantOnBoardingHelper.TenantOnBoarding(TenantDetails, Request.Form.Files);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get Tenant list by type
    /// </summary>
    /// <param name="tenantType"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{env}")]
    public async Task<ActionResult> Get(string tenantType)
    {
        try
        {
            var response = await _tenantOnBoardingHelper.GetTenantList(tenantType);
            return Ok(response); // CodeQL [SM04901] justification: The tenantType parameter is not user input but is selected from a predefined list of legitimate options. Data is returned only if tenantType is a valid and legitimate option. It is unrelated to the "tid" claim.
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}