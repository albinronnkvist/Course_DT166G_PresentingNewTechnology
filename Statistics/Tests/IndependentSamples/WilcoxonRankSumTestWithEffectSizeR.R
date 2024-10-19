group1 <- c(50, 52, 54, 56, 58, 60, 62)
group2 <- c(65, 67, 70, 72, 74, 75, 77)

wilcox_test <- wilcox.test(group1, group2)

# Extract test statistic (W)
W <- wilcox_test$statistic


# Calculate Z-score
z <- qnorm(wilcox_test$p.value / 2)

# Calculate total number of observations
N <- length(group1) + length(group2)

# Calculate effect size r
r <- z / sqrt(N)

cat("Effect size (r):", abs(r), "\n")