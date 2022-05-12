﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Mc.Service.Admin.Application.MessageInfos.Commands;

public class CreateMessageInfoCommandValidator : AbstractValidator<CreateMessageInfoCommand>
{
    public CreateMessageInfoCommandValidator() => RuleFor(cmd => cmd.MessageInfo).SetValidator(new MessageInfoUpsertDtoValidator());
}