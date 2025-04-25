public class Program
{
    public static void Main(string[] args)
    {
        NeuralNetwork nn = new NeuralNetwork(new int[] { 2, 3, 2 });

        double[] input = new double[] { 1.0, 0.5 };

        double[] outputs = nn.CalculateOutputs(input);


        // Console.WriteLine("Çıktılar:");
        // foreach (double output in outputs)
        // {
        //     Console.WriteLine(output);
        // }

        // Classify fonksiyonu private, onu da public yaparsak:
        int predictedClass = nn.Classify(input);
        var decision = predictedClass == 0 ? "Not Poisoned" : "Poisoned";
        Console.WriteLine($"Tahmin edilen sınıf: {predictedClass}, {decision}");
    }
}


public class Layer
{
    public int numNodesIn, numNodesOut;
    public double[,] weights;
    double[] biases;

    public Layer(int numNodesIn, int numNodesOut)
    {
        this.numNodesIn = numNodesIn;
        this.numNodesOut = numNodesOut;
        weights = new double[numNodesIn, numNodesOut];
        biases = new double[numNodesOut];

        Random rnd = new Random();
        for (int j = 0; j < numNodesOut; j++)
        {
            for (int i = 0; i < numNodesIn; i++)
            {
                var num2 = rnd.NextDouble();
                weights[i, j] = Math.Round(num2, 2, MidpointRounding.AwayFromZero);
            }
            var num = rnd.NextDouble();
            biases[j] = Math.Round(num, 2, MidpointRounding.AwayFromZero);
        }
    }

    public double[] CalculateOutputs(double[] inputs)
    {
        double[] weightedInputs = new double[numNodesOut];

        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            int tempOut = nodeOut + 1;
            double weightedInput = biases[nodeOut];
            Console.WriteLine($"Bias[{tempOut}]: {biases[nodeOut]}");
            for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
            {
                int tempIn = nodeIn + 1;
                weightedInput += inputs[nodeIn] * weights[nodeIn, nodeOut];
                Console.WriteLine($"Ağırlık[{tempIn},{tempOut}]: {weights[nodeIn, nodeOut]},        Giriş: {inputs[nodeIn]},        Çıkış: {weightedInput}");
            }
            weightedInputs[nodeOut] = weightedInput;
        }
        return weightedInputs;
    }
}
public class NeuralNetwork
{
    Layer[] layers;

    public NeuralNetwork(int[] layerSizes)
    {
        layers = new Layer[layerSizes.Length - 1];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layerSizes[i], layerSizes[i + 1]);
        }
    }

    public double[] CalculateOutputs(double[] inputs)
    {
        int layerNum = 0;
        foreach (Layer layer in layers)
        {
            Console.WriteLine($"Layer{layerNum}");
            inputs = layer.CalculateOutputs(inputs);
            layerNum++;
        }
        return inputs;
    }

    public int Classify(double[] inputs)
    {
        double[] outputs = CalculateOutputs(inputs);
        int maxIndex = 0;
        for (int i = 1; i < outputs.Length; i++)
        {
            if (outputs[i] > outputs[maxIndex])
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }
}