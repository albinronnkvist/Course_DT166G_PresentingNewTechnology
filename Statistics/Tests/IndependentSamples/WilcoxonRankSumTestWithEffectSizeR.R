group1 <- c(50, 52, 54, 56, 58, 60, 62)
group2 <- c(65, 67, 70, 72, 74, 75, 77)

wilcox_test <- wilcox.test(group1, group2)

## Calculate effect size r
# Calculate Z-score
z <- qnorm(wilcox_test$p.value / 2)

# Calculate total number of observations
N <- length(group1) + length(group2)

# Calculate effect size r
r <- z / sqrt(N)

## Calculate medians
median_group1 <- median(group1)
median_group2 <- median(group2)

## Print results
print(wilcox_test)
cat("Median of Group 1:", median_group1, "\n")
cat("Median of Group 2:", median_group2, "\n")
cat("Effect size (r):", abs(r), "\n")