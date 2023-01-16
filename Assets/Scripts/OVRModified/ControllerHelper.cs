/************************************************************************************
Copyright : Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.

Your use of this SDK or tool is subject to the Oculus SDK License Agreement, available at
https://developer.oculus.com/licenses/oculussdk/

Unless required by applicable law or agreed to in writing, the Utilities SDK distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
ANY KIND, either express or implied. See the License for the specific language governing
permissions and limitations under the License.
************************************************************************************/

using UnityEngine;

/// <summary>
/// Simple helper script that conditionally enables rendering of a controller if it is connected.
/// </summary>
public class ControllerHelper : MonoBehaviour {

	public GameObject m_modelOculusTouchQuest2LeftController;

	/// <summary>
	/// The root GameObject that represents the Oculus Touch for Quest 2 Controller model (Right).
	/// </summary>
	public GameObject m_modelOculusTouchQuest2RightController;

	/// <summary>
	/// The controller that determines whether or not to enable rendering of the controller model.
	/// </summary>
	public OVRInput.Controller m_controller;

	/// <summary>
	/// The animator component that contains the controller animation controller for animating buttons and triggers.
	/// </summary>
	private Animator m_animator;

	private enum ControllerType {
		QuestAndRiftS = 1,
		Rift = 2,
		Quest2 = 3,
	}

	private ControllerType activeControllerType = ControllerType.Rift;

	private bool m_prevControllerConnected = false;
	private bool m_prevControllerConnectedCached = false;

	void Start() {
		OVRPlugin.SystemHeadset headset = OVRPlugin.GetSystemHeadsetType();
		switch (headset) {
			case OVRPlugin.SystemHeadset.Rift_CV1:
				activeControllerType = ControllerType.Quest2;
				break;
			case OVRPlugin.SystemHeadset.Oculus_Quest_2:
				activeControllerType = ControllerType.Quest2;
				break;
			default:
				activeControllerType = ControllerType.Quest2;
				break;
		}

		Debug.LogFormat("OVRControllerHelp: Active controller type: {0} for product {1}", activeControllerType, OVRPlugin.productName);

		// Hide all controller models until controller get connected
		m_modelOculusTouchQuest2LeftController.SetActive(false);
		m_modelOculusTouchQuest2RightController.SetActive(false);
	}

	void Update() {
		bool controllerConnected = OVRInput.IsControllerConnected(m_controller);

		if ((controllerConnected != m_prevControllerConnected) || !m_prevControllerConnectedCached) {
			m_modelOculusTouchQuest2LeftController.SetActive(controllerConnected && (m_controller == OVRInput.Controller.LTouch));
			m_modelOculusTouchQuest2RightController.SetActive(controllerConnected && (m_controller == OVRInput.Controller.RTouch));

			m_animator = m_controller == OVRInput.Controller.LTouch ? m_modelOculusTouchQuest2LeftController.GetComponent<Animator>() :
				m_modelOculusTouchQuest2RightController.GetComponent<Animator>();
			m_prevControllerConnected = controllerConnected;
			m_prevControllerConnectedCached = true;
		}

		if (m_animator != null) {
			m_animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, m_controller) ? 1.0f : 0.0f);
			m_animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, m_controller) ? 1.0f : 0.0f);
			m_animator.SetFloat("Button 3", OVRInput.Get(OVRInput.Button.Start, m_controller) ? 1.0f : 0.0f);

			m_animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).x);
			m_animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).y);

			m_animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller));
			m_animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller));
		}
	}

	public void EnableMesh(string enabled) {
		bool controllerConnected = OVRInput.IsControllerConnected(m_controller);
		switch (enabled) {
			case "Enabled":
			case "enabled":
				m_modelOculusTouchQuest2LeftController.SetActive(controllerConnected && (m_controller == OVRInput.Controller.LTouch));
				m_modelOculusTouchQuest2RightController.SetActive(controllerConnected && (m_controller == OVRInput.Controller.RTouch));
				break;
			case "Disabled":
			case "disabled":
				m_modelOculusTouchQuest2LeftController.SetActive(false);
				m_modelOculusTouchQuest2RightController.SetActive(false);
				break;
		}
	}
}
