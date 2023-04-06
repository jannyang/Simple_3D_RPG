using GenshinImpactMovementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GenshinImpactMovementSystem
{
    public class PlayerComboAttackState : PlayerAttackingState
    {
        bool alreadyApplyCombo = false;
        bool alreadyAppliedForce = false;

        Vector3 currentPosition;
        Vector3 targetPosition;

        float startTime;
        float endTime;

        AttackInfoData attackInfoData;


        public PlayerComboAttackState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            int comboStateIndex = stateMachine.ReusableData.ComboStateIndex;
            attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfo(comboStateIndex);

            alreadyApplyCombo = false;
            alreadyAppliedForce = false;

            StartAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);
            stateMachine.Player.Animator.SetInteger("Combo", comboStateIndex);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

            if (!alreadyApplyCombo)
            {
                stateMachine.ReusableData.ComboStateIndex = 0;
            }
        }

        public override void Update()
        {
            base.Update();
            
            float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
            if (normalizedTime >= 1f)
            {
                if (alreadyApplyCombo)
                {
                    ApplyCombo();
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.IdlingState); 
                }
            }
            else
            {
                if (normalizedTime >= attackInfoData.ForceTransitionTime)
                    AddForceTrigger();

                if (normalizedTime >= attackInfoData.ComboTransitionTime)
                    ComboAttackTrigger(normalizedTime);
            }


        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (alreadyAppliedForce)
                ApplyForce();
        }


        private void ComboAttackTrigger(float normalizedTime)
        {
            if (alreadyApplyCombo) return;

            if (stateMachine.ReusableData.AttackInput)
                alreadyApplyCombo = true;
        }

        private void ApplyCombo()
        {
            stateMachine.ReusableData.ComboStateIndex = attackInfoData.ComboStateIndex;
            stateMachine.ChangeState(stateMachine.ComboAttackingState);
        }

        private void AddForceTrigger()
        {
            if (alreadyAppliedForce) return;
            startTime = Time.time;
            endTime = startTime + attackInfoData.ForceTime;
            currentPosition = stateMachine.Player.transform.position;
            targetPosition = currentPosition + stateMachine.Player.transform.forward * attackInfoData.ForceDistance;

            alreadyAppliedForce = true;
        }

        private void ApplyForce()
        {
            float nowTime = Time.time;
            if (nowTime > endTime)
                return;

                float normalizedTime = (Time.time - startTime) / attackInfoData.ForceTime;
                Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, normalizedTime);

                stateMachine.Player.Rigidbody.MovePosition(newPosition);
            
        }

    }
}