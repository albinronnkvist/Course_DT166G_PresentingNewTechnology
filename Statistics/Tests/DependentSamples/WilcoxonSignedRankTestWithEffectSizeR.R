group1 <- c(50, 52, 54, 56, 58, 60, 62)
group2 <- c(65, 67, 70, 72, 74, 75, 77)

wilcox_test <- wilcox.test(group1, group2, paired = TRUE)

n <- length(group1)
Z <- qnorm(wilcox_test$p.value / 2)
r <- abs(Z) / sqrt(n)

cat("Effect size (r):", round(r, 3), "\n")