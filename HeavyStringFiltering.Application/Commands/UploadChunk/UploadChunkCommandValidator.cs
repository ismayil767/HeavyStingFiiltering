using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavyStringFiltering.Application.Commands.UploadChunk
{
    public class UploadChunkCommandValidator : AbstractValidator<UploadChunkCommand>
    {
        public UploadChunkCommandValidator()
        {
            RuleFor(x => x.UploadId).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
            RuleFor(x => x.ChunkIndex).GreaterThanOrEqualTo(0);
        }
    }
}
