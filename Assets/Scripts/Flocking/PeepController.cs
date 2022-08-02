using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Avrahamy;
using Avrahamy.Math;

namespace Flocking {
    public class PeepController : MonoBehaviour {
        [SerializeField] int group;
        [SerializeField] float maxSpeed = 10f;
        [Range(0f, 1f)] 
        [SerializeField] float rotationSpeed = 0.2f;
        [SerializeField] float acceleration = 2f;
        [SerializeField] float deceleration = 1f;
        [SerializeField] float minSqrSpeed = 0.1f;
        [SerializeField] private List<Material> groupMaterials;
        
        [SerializeField] private bool changeWeights;
        private float safeCoolDown = 5f;
        private float protectCoolDown = 7f;
        private bool canJoinGroup = true;
        private float timeForSeperarion;
        public bool canBeHarmed = true;


        public int Group {
            get {
                return group;
            }
            set {
                group = value;
            }
        }

        public float MaxSpeed {
            get {
                return maxSpeed;
            }
            set {
                maxSpeed = value;
            }
        }

        public float Acceleration {
            get {
                return acceleration;
            }
            set {
                acceleration = value;
            }
        }

        public Vector3 Position {
            get {
                return body.position;
            }
            set {
                body.position = value;
            }
        }

        public Vector2 Velocity {
            get {
                return body.velocity.ToVector2XZ();
            }
            set {
                body.velocity = value.ToVector3XZ();
            }
        }

        public Vector2 DesiredVelocity {
            get {
                return desiredVelocity;
            }
            set {
                desiredVelocity = value;
            }
        }

        public float SelfRadius {
            get {
                return selfRadius;
            }
        }

        private Rigidbody body;
        private Vector2 desiredVelocity;
        private float selfRadius;

        protected void Awake() {
            body = GetComponent<Rigidbody>();
            selfRadius = GetComponentInChildren<CapsuleCollider>().radius;
        }

        protected void Update() {
            DebugDraw.DrawArrowXZ(
                Position + Vector3.up,
                desiredVelocity.ToVector3XZ() * 3f,
                1f,
                30f,
                group == 0 ? Color.red : Color.blue);


            if (changeWeights)
            {
                timeForSeperarion += Time.deltaTime;
            }
              
            if (timeForSeperarion > 2.5f)
            {
                FollowerPeepController.redWeights = new Vector3(0f, 0.75f, 0.25f);
                FollowerPeepController.blueWeights = new Vector3(0f, 0.75f, 0.25f);
                changeWeights = false;
                timeForSeperarion = 0;
            }
        }
        

        protected void FixedUpdate() {
            var currentVelocity = Velocity;
            if (desiredVelocity.sqrMagnitude > 0.1f) {
                var targetRotation = Quaternion.LookRotation(desiredVelocity.ToVector3XZ(), Vector3.up);
                body.rotation = Quaternion.LerpUnclamped(body.rotation, targetRotation, rotationSpeed);
                Velocity = Vector2.MoveTowards(currentVelocity, desiredVelocity * maxSpeed, acceleration);
            } else if (currentVelocity.sqrMagnitude > minSqrSpeed) {
                Velocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration);
            } else {
                Velocity = Vector2.zero;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Steal"))
            {
                other.gameObject.SetActive(false);
                var findPeeps = FindObjectsOfType<PeepController>();
                foreach (var peep in findPeeps)
                {
                    if (peep.@group != @group &&  peep.@group != 2 && peep.gameObject.layer != 7)
                    {
                        peep.group = @group;
                        var peepRenderer = peep.gameObject.GetComponentInChildren<MeshRenderer>();
                        peepRenderer.material = GameManager._shared.regularMaterials[@group];
                        break;
                    }
                }
            }

            if (other.gameObject.CompareTag("Protect") && group != 2)
            {
                other.gameObject.SetActive(false);
                var findPeeps = FindObjectsOfType<PeepController>();
                // var findPeeps = FindObjectOfType<PeepController>();
                foreach (var peep in findPeeps)
                {
                    if (peep.@group == @group && peep.@group != 2)
                    {
                        var peepRenderer = peep.gameObject.GetComponentInChildren<MeshRenderer>();
                        peepRenderer.material = GameManager._shared.ghostMaterials[@group];
                        StartCoroutine(peep.PeepSafe(protectCoolDown));
                    }
                }
            }
            
            else if (other.gameObject.CompareTag("Peep"))
            {
                var peep = other.gameObject.GetComponent<PeepController>();
                if (!peep.canJoinGroup || peep == null)
                {
                    return;
                }
                if (peep.group == 2)
                {
                    peep.maxSpeed = maxSpeed;
                    peep.group = @group;
                    var peepRenderer = other.gameObject.GetComponentInChildren<MeshRenderer>();
                    peepRenderer.material = gameObject.GetComponentInChildren<MeshRenderer>().material;
                    StartCoroutine(peep.PeepSafe(safeCoolDown));
                }
            }
            else if (other.gameObject.CompareTag("Thorns"))
            {
                if (@group == 0)
                    FollowerPeepController.redWeights = new Vector3(1,0,0);
                else if (@group == 1)
                    FollowerPeepController.blueWeights = new Vector3(1,0,0);
                changeWeights = true;
            }
            
            else if (other.gameObject.CompareTag("Final"))
            {
                if (Score.blueScore >= 10 && @group == 1)
                {
                    Score.groupWinner = 1;
                    Score.GameOver();
                }
                else if (Score.redScore >= 10 && @group == 0)
                {
                    Score.groupWinner = 0;
                    Score.GameOver();
                }
            }
        }

        public IEnumerator CantJoin(float coolDown)
        {
            if (canJoinGroup)
            {
                canJoinGroup = false;
                yield return new WaitForSeconds(coolDown);
                var peepRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
                peepRenderer.material = GameManager._shared.greenMaterial;
                canJoinGroup = true;
            }
        }

        public IEnumerator PeepSafe(float safeTimer)
        {
            canBeHarmed = false;
            yield return new WaitForSeconds(safeTimer);
            var peepRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            if (group != 2)
            {
                peepRenderer.material = GameManager._shared.regularMaterials[@group];

            }
            canBeHarmed = true;
        }
    }
    
}
