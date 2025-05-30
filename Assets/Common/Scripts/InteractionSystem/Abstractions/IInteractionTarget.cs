using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarenKrill.InteractionSystem.Abstractions
{
    public interface IInteractionTarget
    {
        IInteractable Interactable { get; }
    }
}
