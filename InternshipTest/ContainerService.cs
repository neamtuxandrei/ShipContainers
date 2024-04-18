using System.Text;

namespace InternshipTest
{
    public class ContainerService
    {
        private readonly Dictionary<int, Stack<Container>> _stacks;
        public ContainerService(Dictionary<int, Stack<Container>> stacks)
        {
            _stacks = stacks;
        }

        public void MoveContainer(int containersToMove, int startingPoint, int endingPoint)
        {
            Container temp;
            for (int i = 0; i < containersToMove; i++)
            {
                // -1 bcs stack starts at 0
                temp = _stacks[startingPoint - 1].Pop();
                _stacks[endingPoint - 1].Push(temp);
            }
        }

        public string GetTopContainersConcatenated()
        {
            StringBuilder concatenatedItems = new StringBuilder();

            foreach (var stack in _stacks)
            {
                var topContainer = stack.Value.Peek().Letter;
                concatenatedItems.Append(topContainer);
            }

            return concatenatedItems.ToString();
        }
    }
}
