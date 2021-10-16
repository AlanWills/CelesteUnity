using Celeste.FSM;
using Celeste.Memory;
using Celeste.Narrative.Choices;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Choice View")]
    public class ChoiceView : NarrativeView
    {
        #region Properties and Fields

        [SerializeField] private GameObjectAllocator textChoicesAllocator;
        [SerializeField] private GameObjectAllocator spriteChoicesAllocator;

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is IChoiceNode;
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            IChoiceNode choiceNode = fsmNode as IChoiceNode;
            for (int i = 0, n = choiceNode.NumChoices; i < n; ++i)
            {
                var choice = choiceNode.GetChoice(i);
                bool shouldAllocate = choice.IsValid() || choice.InvalidBehaviour == InvalidBehaviour.ShowDisabled;

                if (shouldAllocate)
                {
                    if (choice is ITextChoice && textChoicesAllocator.CanAllocate(1))
                    {
                        GameObject choiceGameObject = textChoicesAllocator.Allocate();
                        TextChoiceController choiceController = choiceGameObject.GetComponent<TextChoiceController>();
                        choiceController.Hookup(choice as ITextChoice, choiceNode.SelectChoice);
                        choiceGameObject.SetActive(true);
                    }
                    else if (choice is ISpriteChoice && spriteChoicesAllocator.CanAllocate(1))
                    {
                        GameObject choiceGameObject = spriteChoicesAllocator.Allocate();
                        SpriteChoiceController choiceController = choiceGameObject.GetComponent<SpriteChoiceController>();
                        choiceController.Hookup(choice as ISpriteChoice, choiceNode.SelectChoice);
                        choiceGameObject.SetActive(true);
                    }
                }
            }
        }

        public override void OnNodeUpdate(FSMNode fsmNode)
        {
        }

        public override void OnNodeExit(FSMNode fsmNode)
        {
            textChoicesAllocator.DeallocateAll();
            spriteChoicesAllocator.DeallocateAll();
        }

        #endregion
    }
}
