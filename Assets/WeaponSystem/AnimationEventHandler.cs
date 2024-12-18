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

        private void AnimationFinishedTrigger() => OnFinish?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void AOEAttackTrigger() => OnAOEAction?.Invoke();
        private void ProjectileTrigger(int amount) => OnProjectileAction?.Invoke();


    }
}
