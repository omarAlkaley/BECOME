using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
	[SerializeField] private Ability[] abilities;

	private PlayerInput playerInput;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		for (int i = 0; i < abilities.Length; i++)
		{
			string actionName = $"Ability{i}";
			InputAction action = playerInput.actions[actionName];

			if (action != null)
			{
				int index = i;

				action.performed += ctx => abilities[index].OnPressed(gameObject);
				action.canceled += ctx => abilities[index].OnReleased(gameObject);
			}
			else
			{
#if UNITY_EDITOR
				Debug.LogWarning($"No action found with name '{actionName}' in PlayerInput.");
#endif
			}
		}
	}
	private void OnDestroy()
	{
		for (int i = 0; i < abilities.Length; i++)
		{
			string actionName = $"Ability{i}";
			InputAction action = playerInput.actions[actionName];

			if (action != null)
			{
				int index = i;
				action.performed -= ctx => abilities[index].OnPressed(gameObject);
				action.canceled -= ctx => abilities[index].OnReleased(gameObject);
			}
		}
	}
}
