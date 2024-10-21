## Load the data
dat = read.csv("models.csv")

# Fetch the needed values
NotUsingModels = dat$valueA
UsingModels = dat$valueB



## Boxplot
# labels for Axis
label=c("Not Using Models","Using Models")
# print the boxplot
boxplot(NotUsingModels, UsingModels, names= label)



## Means and standard deviations
mean.NotUsingModels = round(mean(NotUsingModels),3)
sd.NotUsingModels = round(sd(NotUsingModels),3)
sprintf("Not Using Models (Mean): %s", mean.NotUsingModels)
sprintf("Not Using Models (SD): %s", sd.NotUsingModels)
mean.UsingModels = round(mean(UsingModels),3)
sd.UsingModels = round(sd(UsingModels),3)
sprintf("Using Models (Mean): %s", mean.UsingModels)
sprintf("Using Models (SD): %s", sd.UsingModels)



## Normality tests & variance tests
# Shapiro-Wilk normality test for the first group
shapiro.test(NotUsingModels)
# Shapiro-Wilk normality test for the second group
shapiro.test(UsingModels)
## F test to compare the two variances
var.test(NotUsingModels, UsingModels)



## Independent t-test (parametric)
statistical.t_test = t.test(NotUsingModels, UsingModels)
statistical.t_test

# Effect size
#  the value of t (t-statistic) is stored as a variable called statistic
t = statistical.t_test$statistic
t
#  the value of df (degrees of freedom) is stored as a variable called parameter
df= statistical.t_test$parameter
df
# Calculate effect size
r = sqrt(t^2/(t^2+df))
round (r, 3)



## Wilcoxon rank-sum test (non parametric)
statistical.wilcox_test = wilcox.test(NotUsingModels, UsingModels)
statistical.wilcox_test

# Effect size
#  number of samples or total observations
N = 25
#  z is the Z-score
z = qnorm(statistical.wilcox_test$p.value/2)
#  effect size equation
r = z/sqrt(N)
#  round the value to 3 decimal places
round (r, 3)