group1 <- c(50, 52, 54, 56, 58, 60, 62)
group2 <- c(65, 67, 70, 72, 74, 75, 77)

t_test <- t.test(group1, group2)

# Calculate effect size r
t_value <- t_test$statistic
df <- t_test$parameter
r <- sqrt(t_value^2/(t_value^2+df))

print(t_test)
cat("Effect size (r):", r, "\n")