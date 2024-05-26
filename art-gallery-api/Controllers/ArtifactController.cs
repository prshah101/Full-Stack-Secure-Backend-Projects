using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using art_gallery_api.Persistence;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/artifacts")]
    public class ArtifactController : ControllerBase
    {
        
        [HttpGet] //Getting All Artifacts
        public IActionResult GetAllArtifacts()
        {
            var artifacts = ArtifactDataAccess.GetAllArtifacts();
            return Ok(artifacts);
        }

        [HttpGet("{id}", Name = "GetArtifact")] //Get an Artifact by ID
        public IActionResult GetArtifactById(int id)
        {
            var artifact = ArtifactDataAccess.GetArtifactById(id);
            if (artifact == null)
            {
                return NotFound();
            }
            return Ok(artifact);
        }


        [HttpPost] //Add an Artifact
        public IActionResult AddArtifact(Artifact newArtifact)
        {
            if (newArtifact == null)
            {
                return BadRequest();
            }

            try
            {
                ArtifactDataAccess.AddArtifact(newArtifact);
                Artifact? addedNewArtifact = ArtifactDataAccess.GetArtifactByName(newArtifact.Name);
                return CreatedAtRoute("GetArtifact", new { id = addedNewArtifact.Id }, addedNewArtifact);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")] //Update an Artifact
        public IActionResult UpdateArtifact(int id, Artifact updatedArtifact)
        {
            var existingArtifact = ArtifactDataAccess.GetArtifactById(id);
            if (existingArtifact == null)
            {
                return NotFound();
            }
            
            try
            {
                ArtifactDataAccess.UpdateArtifact(id, updatedArtifact);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")] //Delete an Artifact
        public IActionResult DeleteArtifact(int id)
        {
            var existingArtifact = ArtifactDataAccess.GetArtifactById(id);
            if (existingArtifact == null)
            {
                return NotFound();
            }

            try
            {
                ArtifactDataAccess.DeleteArtifact(id);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
