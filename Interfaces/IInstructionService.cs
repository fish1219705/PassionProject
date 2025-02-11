using PassionProject.Models;

namespace PassionProject.Interfaces
{
    public interface IInstructionService
    {

        // base CRUD
        Task<IEnumerable<InstructionDto>> ListInstructions();
        Task<InstructionDto?> FindInstruction(int id);
        Task<ServiceResponse> UpdateInstruction(InstructionDto instructionDto);
        Task<ServiceResponse> AddInstruction(InstructionDto instructionDto);
        Task<ServiceResponse> DeleteInstruction(int id);

        // related methods

        Task<IEnumerable<InstructionDto>> ListInstructionsForDessert(int id);
        Task<IEnumerable<InstructionDto>> ListInstructionsForIngredient(int id);

    }
}
