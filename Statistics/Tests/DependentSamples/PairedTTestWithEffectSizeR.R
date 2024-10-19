pre_test <- c(50, 55, 60, 62, 70, 72, 75)
post_test <- c(52, 58, 63, 66, 72, 76, 80)

t_test <- t.test(pre_test, post_test, paired = TRUE)

t_value <- t_test$statistic
df <- t_test$parameter

# Calculate effect size r
r <- sqrt(t_value^2/(t_value^2+df))

cat("t-value:", t_value, "\n")
cat("Degrees of freedom (df):", df, "\n")
cat("Effect size (r):", r, "\n")