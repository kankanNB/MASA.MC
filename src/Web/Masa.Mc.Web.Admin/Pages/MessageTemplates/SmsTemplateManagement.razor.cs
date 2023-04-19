﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Mc.Web.Admin.Pages.MessageTemplates;

public partial class SmsTemplateManagement : AdminCompontentBase
{
    public List<DataTableHeader<MessageTemplateDto>> Headers { get; set; } = new();

    private SmsTemplateEditModal _editModal = default!;
    private SmsTemplateCreateModal _createModal = default!;
    private GetMessageTemplateInputDto _queryParam = new() { ChannelType = ChannelTypes.Sms };
    private PaginatedListDto<MessageTemplateDto> _entities = new();
    private List<ChannelDto> _channelItems = new();
    private bool _advanced = false;
    private bool _isAnimate;
    private DateOnly? _endTime;
    private DateOnly? _startTime;

    private ChannelService ChannelService => McCaller.ChannelService;

    private MessageTemplateService MessageTemplateService => McCaller.MessageTemplateService;

    protected async override Task OnInitializedAsync()
    {
        const string prefix = "DisplayName.MessageTemplate";
        Headers = new()
        {
            new() { Text = T($"{prefix}{nameof(MessageTemplateDto.Code)}"), Value = nameof(MessageTemplateDto.Code), Sortable = false, Width = "13.125rem" },
            new() { Text = T($"{prefix}{nameof(MessageTemplateDto.DisplayName)}"), Value = nameof(MessageTemplateDto.DisplayName), Sortable = false, Width = "13.125rem"},
            new() { Text = T($"{prefix}{nameof(MessageTemplateDto.TemplateType)}"), Value = nameof(MessageTemplateDto.TemplateType), Sortable = false, Width = "6.5625rem" },
            new() { Text = T($"{prefix}ChannelDisplayName"), Value = "ChannelDisplayName", Sortable = false, Width = "6.5625rem" },
            new() { Text = T($"{prefix}{nameof(MessageTemplateDto.AuditStatus)}"), Value = nameof(MessageTemplateDto.AuditStatus), Sortable = false, Width = "6.5625rem" },
            new() { Text = T("Modifier"), Value = nameof(MessageTemplateDto.ModifierName), Sortable = false, Width = "6.5625rem" },
            new() { Text = T("ModificationTime"), Value = nameof(MessageTemplateDto.ModificationTime), Sortable = true, Width = "6.5625rem" },
             new() { Text = T($"{prefix}{nameof(MessageTemplateDto.Status)}"), Value = nameof(MessageTemplateDto.Status), Sortable = true, Width = "6.5625rem" },
            new() { Text = T("Action"), Value = "Action", Sortable = false, Width = 105, Align = DataTableHeaderAlign.Center },
        };
        _channelItems = await ChannelService.GetListByTypeAsync(ChannelTypes.Sms);
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadData();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private Task OnDateChanged((DateOnly? startDate, DateOnly? endDate) args)
    {
        (_startTime, _endTime) = args;
        _queryParam.StartTime = _startTime?.ToDateTime(TimeOnly.MinValue);
        _queryParam.EndTime = _endTime?.ToDateTime(TimeOnly.MaxValue);
        return RefreshAsync();
    }

    private async Task LoadData()
    {
        Loading = true;
        _entities = (await MessageTemplateService.GetListAsync(_queryParam));
        Loading = false;
        StateHasChanged();
    }

    private async Task HandleOk()
    {
        await LoadData();
    }

    private async Task RefreshAsync()
    {
        _queryParam.Page = 1;
        await LoadData();
    }

    private async Task HandlePageChanged(int page)
    {
        _queryParam.Page = page;
        await LoadData();
    }

    private async Task HandlePageSizeChanged(int pageSize)
    {
        _queryParam.PageSize = pageSize;
        await LoadData();
    }

    private async Task HandleClearAsync()
    {
        _queryParam = new() { ChannelType = ChannelTypes.Sms };
        await LoadData();
    }

    private void ToggleAdvanced()
    {
        _advanced = !_advanced;
        _isAnimate = true;
    }
}
