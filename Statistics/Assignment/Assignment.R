### Pre-requisites
# Load data
data <- read.csv("assignment.csv")

# Fetch the required values for the analysis
# N = no onboarding, O = onboarding
productivityN <- data$productivityN
jobSatisfactionN <- data$jobSatisfactionN
productivityO <- data$productivityO
jobSatisfactionO <- data$jobSatisfactionO



### Investigation 1 - Compare means
## Means and standard deviations
productivityN.Mean = round(mean(productivityN),3)
productivityN.Sd = round(sd(productivityN),3)
productivityO.Mean = round(mean(productivityO),3)
productivityO.Sd = round(sd(productivityO),3)
cat("Productivity N (Mean): ", productivityN.Mean, "\n") # 57.187
cat("Productivity N (SD): ", productivityN.Sd, "\n") # 6.728
cat("Productivity O (Mean): ", productivityO.Mean, "\n") # 80.844
cat("Productivity O (SD): ", productivityO.Sd, "\n") # 8.054
# Productivity O mean and standard deviation is higher than Productivity N. 
# Indicating that O is more productive than N, and O is also slightly more spread out.

## Boxplot
# I will use a boxplot to vizualize the above statements
boxplot(productivityN, productivityO)

## Testing assumptions to decide which test to use to compare the two means
# Shapiro-Wilk normality test for the first group
productivityN.Normality <- shapiro.test(productivityN)
# Shapiro-Wilk normality test for the second group
productivityO.Normality <- shapiro.test(productivityO)

print(productivityN.Normality) # W = 0.94364, p-value = 0.1796. (p > 0.05, so normalized)
print(productivityO.Normality) # W = 0.93512, p-value = 0.1142. (p > 0.05, so normalized)
# Result: The p-value from the Shapiro-Wilk tests are all greater than 0.05, 
# so I can assume the data is normally distributed and proceed with parametric tests.

## F test to compare the two variances (Skipped)
# I will not perform the an F-test since, since the t-test used in the next step uses Welch’s test by default,
# which corrects potential differences in variances.

## Paired t-test (parametric)
# Since the groups are dependent and the data is normalized (assumptions are met, assuming usage of Welch's varaince correction later), 
# I choose the paired t-test.
tTest <- t.test(productivityN, productivityO, paired = TRUE)
print(tTest) # t = -12.647, df = 24, p-value = 4.183e-12
# Result: The p-value from the t-test is less than 0.05, so we can reject the null hypothesis that the means are equal.

## Effect size
tValue <- tTest$statistic
df <- tTest$parameter
r <- sqrt(tValue^2/(tValue^2+df))
cat("Effect size: ", r, "\n") # 0.9324842
# Result: The effect size is > 0.5, and close to 1, indicating a large effect.

## Final result
# Using onboarding improved productivity (M = 80.844, SD = 8.054) in comparison to productivity without onboarding (M = 57.187, SD = 6.728)
# The difference is statistically significant: p-value = 4.183e-12, which is smaller than alpha (α = 0.05). 
# The effect size is large, r = 0.9324842.

# -------------------------

### Investigation 2 - Correlation

# Scatter plot to vizualize
plot(productivityN, jobSatisfactionN)
abline(lm(jobSatisfactionN ~ productivityN))
plot(productivityO, jobSatisfactionO)
abline(lm(jobSatisfactionO ~ productivityO))

## Testing assumptions to decide which correlation test to use
# We already know that the productivity columns are normally distributed.
# But we need to make sure the job satisfaction columns are also normally distributed.
jobSatisfactionN.Normality <- shapiro.test(jobSatisfactionN)
jobSatisfactionO.Normality <- shapiro.test(jobSatisfactionO)
print(jobSatisfactionN.Normality) # W = 0.95522, p-value = 0.3275 (p > 0.05, so normalized)
print(jobSatisfactionO.Normality) # W = 0.95951, p-value = 0.4047 (p > 0.05, so normalized)
# Result: The p-value from the Shapiro-Wilk tests are all greater than 0.05, 
# so I can assume the data is normally distributed and proceed with parametric tests.

# F-test for variance
var.test(productivityN, jobSatisfactionN) # F = 0.86503, num df = 24, denom df = 24, p-value = 0.7253
var.test(productivityO, jobSatisfactionO) # F = 1.1395, num df = 24, denom df = 24, p-value = 0.7516
# Results: the high p-values suggests that the variances are equal.

## Pearson's Test
# I choose Pearson's Test since all assumptions are met (normality and similar variance)
correlationTestN <- cor.test(productivityN, jobSatisfactionN, method = "pearson")
correlationTestO <- cor.test(productivityO, jobSatisfactionO, method = "pearson")

print(correlationTestN) # t = -0.8747, df = 23, p-value = 0.3908, cor -0.1794275
# The correlation coefficient suggests a weak negative relationship between the two variables.
# Since this p-value is greater than 0.05, it suggests that there is no statistically significant correlation between the two variables.
print(correlationTestO) # t = 13.574, df = 23, p-value = 1.82e-12, cor 0.9428797
# The correlation coefficient suggests a very strong positive relationship between the two variables.
# The p-value is less than 0.05, suggesting statistically significant correlation between the two variables.

## Final result
# N: There is a weak negative correlation between productivity and job satisfaction of the employees who are not involved in onboarding.
# The correlation coefficient is -0.1794275. The p-value = 0.3908, which is greater than the significance level of 0.05. The relationship is not statistically significant.

# O: There is a strong positive correlation betwen productivity and job satisfaction of the employees who are involved in onboarding. 
# The correlation coefficient is +0.9428797. The p-value = 1.82e-12, which is smaller than the significance level of 0.05. The relationship is statistically significant.