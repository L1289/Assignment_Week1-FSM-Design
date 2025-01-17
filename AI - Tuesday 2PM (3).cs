using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class ApproachAT : ActionTask {
		public Transform target;
		private float speed;
		public float arrivalDistance;
		public float chargeUseRate;

		private Blackboard agentBlackboard;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			

			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
            agentBlackboard = agent.GetComponent<Blackboard>();
            speed = agentBlackboard.GetVariableValue<float>("speed");
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			float charge = agentBlackboard.GetVariableValue<float>("charge");
			charge -= chargeUseRate * Time.deltaTime;

			agentBlackboard.SetVariableValue("charge", charge);

			Vector3 moveDirection = (target.position - agent.transform.position).normalized;
			agent.transform.position += moveDirection * speed * Time.deltaTime;

			float distanceToTarget = Vector3.Distance(target.position, agent.transform.position);
			if(distanceToTarget < arrivalDistance)
			{
				EndAction(true);
			}

		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compactor : MonoBehaviour
{
    public Transform pressPad;
    public Animator animator;

    public void Crush()
    {
        animator.SetBool("IsOn", true);
    }

    public void Stop()
    {
        animator.SetBool("IsOn", false);
    }
}

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class CrushAT : ActionTask {

		public Compactor compactor;
		public float crushDuration;

		private float timeCrushing = 0f;

        public float chargeUseRate;

        private Blackboard agentBlackboard;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit() {
            agentBlackboard = agent.GetComponent<Blackboard>();
            return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			compactor.Crush();
			timeCrushing = 0f;
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			timeCrushing += Time.deltaTime;

            float charge = agentBlackboard.GetVariableValue<float>("charge");
            charge -= chargeUseRate * Time.deltaTime;

            agentBlackboard.SetVariableValue("charge", charge);

            if (timeCrushing > crushDuration)
			{
				EndAction(true);
				compactor.Stop();
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class RechargeAT : ActionTask {
		public float maxCharge;
		public float rechargeRate;

		public BBParameter<float> charge;
		public BBParameter<GameObject> powerStationObject;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
            Blackboard powerStationBlackboard = powerStationObject.value.GetComponent<Blackboard>();
			Debug.Log("We are charging at: " + powerStationBlackboard.GetVariableValue<string>("powerStationName"));
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			charge.value += rechargeRate * Time.deltaTime;

			if(charge.value >= maxCharge)
			{
				charge.value = maxCharge;
				EndAction(true);
			}
        }

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}

using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions {

	public class ValueUnderThresholdCT : ConditionTask {
		public string variableName;
		public float threshold;

		private Blackboard agentBlackboard;


		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			agentBlackboard = agent.GetComponent<Blackboard>();

			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable() {
			
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable() {
			
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck() {
			float value = agentBlackboard.GetVariableValue<float>(variableName);
			bool isUnderThreshold = value < threshold;

            return isUnderThreshold;
		}
	}
}