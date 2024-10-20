pre_test <- c(50, 55, 60, 62, 70, 72, 75)
post_test <- c(52, 58, 63, 66, 72, 76, 80)

t_test <- t.test(pre_test, post_test, paired = TRUE)

# Calculate effect size r
t_value <- t_test$statistic
df <- t_test$parameter
r <- sqrt(t_value^2/(t_value^2+df))

## Means and standard deviations
mean.pre_test = round(mean(pre_test),3)
sd.pre_test = round(sd(pre_test),3)
mean.post_test = round(mean(post_test),3)
sd.post_test = round(sd(post_test),3)

print(t_test)
cat("Pre-test (Mean): ", mean.pre_test, "\n")
cat("Pre-test (SD): ", sd.pre_test, "\n")
cat("Post-test (Mean): ", mean.post_test, "\n")
cat("Post-test (SD): ", sd.post_test, "\n")
cat("Effect size (r):", r, "\n")