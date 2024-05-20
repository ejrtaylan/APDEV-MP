
public class Stat {
    public int Score;
    public int Bonus;
    
    public int Mod(){
        return (int)((this.Score + this.Bonus - 10) / 2);
    }
}
