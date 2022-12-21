using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class PlayerAttributes : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider rayCollider;
        [SerializeField] private float movementSpeedProperty;
        [SerializeField] private GameObject rayModel;
        [SerializeField] private ParticleSystem rayRing;
        [SerializeField] private ParticleSystem rayTractor;
        public float MovementSpeed
        {
            get { return movementSpeedProperty; }
        }

        [SerializeField] private float rayForceProperty;
        public float RayForce
        {
            get { return rayForceProperty; }
        }

        [SerializeField] private float rayColliderRadiusProperty;
        public float RayColliderRadius
        {
            get { return rayColliderRadiusProperty; }
        }

        [SerializeField] private Vector3 rayModelScaleProperty;
        public Vector3 RayModelScale
        {
            get { return rayModelScaleProperty; }
        }

        [SerializeField] private float rayColliderCenterYProperty;
        public float RayColliderCenterY
        {
            get { return rayColliderCenterYProperty; }
        }

        [SerializeField] private float rayColliderHeightProperty;
        public float RayColliderHeight
        {
            get { return rayColliderHeightProperty; }
        }

        [SerializeField] private int rayMultiplierProperty;
        public int RayMultiplier
        {
            get { return rayMultiplierProperty; }
        }

        [SerializeField] private float maxEnergyProperty;
        public float MaxEnergy
        {
            get { return maxEnergyProperty; }
        }

        [SerializeField] private float currentEnergyProperty;
        public float CurrentEnergy
        {
            get { return currentEnergyProperty; }
        }

        [SerializeField] private float energyConsumeProperty;
        public float EnergyConsume
        {
            get { return energyConsumeProperty; }
        }

        public void InitializeAttributes()
        {
            rayCollider = GetComponent<CapsuleCollider>();
            //rayModel = transform.Find("Raio").gameObject;
            if (GameControl.Instance == null)
            {
                //Inicializa atributos 
                movementSpeedProperty = 5;
                rayForceProperty = 120;
                rayColliderRadiusProperty = 2;
                rayModelScaleProperty = new Vector3(1, 1, 1);
                var mainRayRing = rayRing.main;
                mainRayRing.startSize = 4f;
                var mainRayTractor = rayTractor.main;
                mainRayTractor.startSize = 4f;
                rayModel.transform.localScale = RayModelScale;
                rayColliderHeightProperty = 14;
                rayColliderCenterYProperty = -RayColliderHeight / 2;
                rayMultiplierProperty = 1;
                maxEnergyProperty = 100;
                currentEnergyProperty = MaxEnergy;
                energyConsumeProperty = 1;


                rayCollider.radius = RayColliderRadius;
                rayCollider.height = RayColliderHeight;
                Vector3 newCenter = new Vector3(0, RayColliderCenterY, 0);
                rayCollider.center = newCenter;

            }
            else
            {
                GameControl control = GameControl.Instance;
                movementSpeedProperty = 5 + control.playerData.playerSpeed;
                rayForceProperty = 120 + (control.playerData.rayForce * 10);
                rayColliderRadiusProperty = 2 + (0.5f * control.playerData.rayRadius);
                rayModelScaleProperty = new Vector3(1 + (0.25f * control.playerData.rayRadius), 1, 1 + (0.25f * control.playerData.rayRadius));
                rayColliderHeightProperty = 14 + (0.5f * control.playerData.rayRadius);
                rayColliderCenterYProperty = -RayColliderHeight / 2;
                rayMultiplierProperty = 1 + control.playerData.rayMultiplier;
                maxEnergyProperty = 100 + (control.playerData.playerEnergy * 20);
                currentEnergyProperty = MaxEnergy;
                energyConsumeProperty = 1 - (0.1f * control.playerData.playerEnergyConsume);

                rayModel.transform.localScale = rayModelScaleProperty;
                rayCollider.radius = RayColliderRadius;
                rayCollider.height = RayColliderHeight;
                Vector3 newCenter = new Vector3(0, RayColliderCenterY, 0);
                rayCollider.center = newCenter;

                var mainRayRing = rayRing.main;
                mainRayRing.startSize = 4f + (float)control.playerData.rayRadius;
                var mainRayTractor = rayTractor.main;
                mainRayTractor.startSize = 4f + (float)control.playerData.rayRadius;
            }
        }
    }
}