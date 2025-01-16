using LibHac.Common.FixedArrays;
using LibHac.FsSystem;
using Ryujinx.Common;
using Ryujinx.Common.Configuration;
using Ryujinx.Common.Logging;
using Ryujinx.Common.Memory;
using Ryujinx.HLE.HOS.Services.Caps.Types;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Services.Caps
{
    [Service("caps:a")]
    class IAlbumAccessorService : IpcService
    {
        public IAlbumAccessorService(ServiceCtx context) { }
        
        
        [CommandCmif(18)]
        // GetAppletProgramIdTable(buffer<nn::caps::ProgramIdTable>) -> bool
        public ResultCode GetAppletProgramIdTable(ServiceCtx context)
        {
            // TODO: Implement this properly.
            Logger.Stub?.PrintStub(LogClass.ServiceCaps);
            context.ResponseData.Write(true);

            return ResultCode.Success;
        }
        
        [CommandCmif(401)]
        // GetAutoSavingStorage()
        public ResultCode GetAutoSavingStorage(ServiceCtx context)
        {
            // TODO: Implement this properly.
            Logger.Stub?.PrintStub(LogClass.ServiceCaps);
            context.ResponseData.Write(false);
            return ResultCode.Success;
        }
    }
}
