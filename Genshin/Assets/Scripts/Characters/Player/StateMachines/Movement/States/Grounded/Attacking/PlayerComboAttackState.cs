using RPGStateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace RPGStateMachineSystem
{
    public class PlayerComboAttackState : PlayerAttackingState
    {
        bool alreadyApplyCombo = false;
        bool alreadyAppliedForce = false;
        
        Vector3 currentPosition;
        Vector3 targetPosition;

        float startTime;
        float endTime;
        float tickTime;

        AttackInfoData attackInfoData;


        public PlayerComboAttackState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
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
                    ComboAttackTrigger();
            }


        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (alreadyAppliedForce)
                ApplyForce();
        }


        private void ComboAttackTrigger()
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
            tickTime = Time.time;

            currentPosition = stateMachine.Player.transform.position;
            currentPosition.y = 0;
            targetPosition = currentPosition + stateMachine.Player.transform.forward * attackInfoData.ForceDistance;
            targetPosition.y = 0;

            alreadyAppliedForce = true;
        }

        private void ApplyForce()
        {
            float nowTime = Time.time;
            if (nowTime > endTime)
                return;

            float perTime = nowTime - tickTime;

            Vector3 playerForward =stateMachine.Player.transform.forward;
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;
            float distance = attackInfoData.ForceDistance * (perTime / attackInfoData.ForceTime);

            Ray downwardsRay = new Ray(capsuleColliderCenterInWorldSpace + (playerForward * distance), Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(new Ray(capsuleColliderCenterInWorldSpace, playerForward * distance), out hit, stateMachine.Player.ResizableCapsuleCollider.SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                return;
            }

            if (!Physics.Raycast(downwardsRay, out hit, stateMachine.Player.ResizableCapsuleCollider.SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                return;
            }

            stateMachine.Player.Rigidbody.MovePosition(hit.point);
            tickTime = nowTime;
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGroundExited(collider);
            Debug.Log("test");
        }


        

    }
}