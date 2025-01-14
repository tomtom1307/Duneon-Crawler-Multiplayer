using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {

        public event Action OnFinish;
        public event Action OnAttackAction;
        public event Action OnAOEAction;
        public event Action OnProjectileAction;
        public event Action NextAttackTrigger;
        

        private void AnimationFinishedTrigger() => OnFinish?.Invoke();
        private void TriggerChainedAttack() => NextAttackTrigger?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void AOEAttackTrigger() => OnAOEAction?.Invoke();
        private void ProjectileTrigger() => OnProjectileAction?.Invoke();


    }
}
