using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Program;

public class EntryPoint
{
    public static void Main(String[] args)
    {
        //ConsoleApp app = new ConsoleApp();
        //app.Run();

        String text = @"This is a sample text that is supposed to be rendered on the jjjjjjjjjjjjjjscreen. Also, this is another line.HAPE RETENTION FOAM -- EVA Foam just LEVELED UP^! Our High Density EVA Foam is specifically formulated to outperform the competition in Durability and Shape Retention. This means that after you pour all of that painstaking effort into your Cosplay, Costuming or Crafting design, you can count on our EVA Foam to hold up to whatever Con or daily activities you throw at your creation! We developed our blend of Shape Retention Foam to make it EASIER for you to work with and SAVE you time.ULTRA HIGH DENSITY CLOSED CELL EVA FOAM, SMOOTH ON BOTH SIDES -- With a true high density of 85 kg/m3 (5.3 lb/ft3), our foam Cuts, Sands, Dremels, Heats and Shapes very well for Cosplay and other designs. Layering is easy for those thicker designs and detail work. No texture or patterns to fight with or have to remove.2MM THICKNESS, 14 X 39 INCH CUT -- 2mm is great for small to medium sized accessories, adding that smaller detail work, and because it is both a thinner 2mm thickness combined with a strong High Density you can even SEW IT! The 14 x 39 inch cut allows for bigger designs, less piecing together, fewer seems and more pieces out of one sheet.BONUS INSTRUCTIONAL PDF -- Scan the QR-code on the package and receive your Downloadable Instructional PDF Guide on how to get started and work with your EVA Foam!BUYING 2 or MORE ROLLS of FOAM? -- Check out our NEW money saving 2-Pack options available in a separate listing, Saving you $10. Click on our The Foamory Storefront above or scroll to the Comparison Chart on this listing featuring our other Products";
        TextEditor textEditor = new TextEditor("MyTitle", text);
        //textEditor.PrintText();
        textEditor.Run();
    }
}
