using BloodShadowFramework.Operations;
using R3;

namespace Test
{
    public class Program
    {
        static async Task Main()
        {
            Subject<(float progress, OperationTaskProgress taskProgress)> oneSubj = new();
            Operation one = new(() => Work(oneSubj), oneSubj);
            Subject<(float progress, OperationTaskProgress taskProgress)> twoSubj = new();
            Operation two = new(() => Work(oneSubj), oneSubj);

            await one;
            Console.WriteLine("Done");
        }

        private static void Work(Subject<(float progress, OperationTaskProgress taskProgress)> subject)
        {
            const int MAX_COUNT = 10000;
            for (int i = 0; i < MAX_COUNT; i++)
            {
                Thread.Sleep(1);
                subject.OnNext(((float)i / MAX_COUNT, OperationTaskProgress.Set));
            }
        }
    }
}
