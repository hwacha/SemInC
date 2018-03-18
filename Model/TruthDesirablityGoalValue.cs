using System;
using System.Collections;
using System.Collections.Generic;

public class TruthDesirabilityGoalValue : ISemanticValue {
    private TruthValue truth;
    private TruthValue desirability;
    private TruthValue goal;

    public TruthDesirabilityGoalValue
        (TruthValue truth, TruthValue desirability, TruthValue goal) {
        this.truth = truth;
        this.desirability = desirability;
        this.goal = goal;
    }

    public int GetID() {
        return -1;
    }

    public TruthValue GetTruth() {
        return truth;
    }

    public TruthValue GetDesirability() {
        return desirability;
    }

    public TruthValue GetGoal() {
        return goal;
    }
}